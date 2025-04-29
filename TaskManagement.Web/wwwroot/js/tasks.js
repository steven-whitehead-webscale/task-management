document.addEventListener('DOMContentLoaded', function() {
    // DOM Elements
    const taskTableBody = document.getElementById('taskTableBody');
    const taskForm = document.getElementById('taskForm');
    const taskIdInput = document.getElementById('taskId');
    const taskTitleInput = document.getElementById('taskTitle');
    const taskDescriptionInput = document.getElementById('taskDescription');
    const taskCompletedInput = document.getElementById('taskCompleted');
    const cancelBtn = document.getElementById('cancelBtn');
    const formTitle = document.getElementById('formTitle');

    // API Base URL
    const apiBaseUrl = '/api/task';

    // Load tasks on page load
    loadTasks();

    // Event Listeners
    cancelBtn.addEventListener('click', resetForm);
    
    // Log when the form submit event listener is added
    console.log('Adding form submit event listener');
    taskForm.addEventListener('submit', function(event) {
        console.log('Form submit event triggered');
        handleFormSubmit(event);
    });

    // Functions
    function loadTasks() {
        console.log('Loading tasks...');
        fetch(apiBaseUrl)
            .then(response => {
                console.log('Response status:', response.status);
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(tasks => {
                console.log('Tasks loaded:', tasks);
                taskTableBody.innerHTML = '';
                if (tasks && tasks.length > 0) {
                    tasks.forEach(task => {
                        const row = document.createElement('tr');
                        row.innerHTML = `
                            <td>${task.id}</td>
                            <td>${task.title}</td>
                            <td>${task.description || ''}</td>
                            <td>${task.isCompleted ? '<span class="badge bg-success">Completed</span>' : '<span class="badge bg-warning">Pending</span>'}</td>
                            <td>
                                <button class="btn btn-sm btn-primary edit-task" data-id="${task.id}">Edit</button>
                                <button class="btn btn-sm btn-danger delete-task" data-id="${task.id}">Delete</button>
                            </td>
                        `;
                        taskTableBody.appendChild(row);
                    });

                    // Add event listeners to edit and delete buttons
                    document.querySelectorAll('.edit-task').forEach(btn => {
                        btn.addEventListener('click', function() {
                            const taskId = this.getAttribute('data-id');
                            editTask(taskId);
                        });
                    });

                    document.querySelectorAll('.delete-task').forEach(btn => {
                        btn.addEventListener('click', function() {
                            const taskId = this.getAttribute('data-id');
                            deleteTask(taskId);
                        });
                    });
                } else {
                    const row = document.createElement('tr');
                    row.innerHTML = '<td colspan="5" class="text-center">No tasks found</td>';
                    taskTableBody.appendChild(row);
                }
            })
            .catch(error => {
                console.error('Error loading tasks:', error);
                alert('Failed to load tasks. Please try again.');
            });
    }

    function resetForm() {
        taskIdInput.value = '';
        taskTitleInput.value = '';
        taskDescriptionInput.value = '';
        taskCompletedInput.checked = false;
        formTitle.textContent = 'Add Task';
    }

    function handleFormSubmit(event) {
        event.preventDefault();
        console.log('handleFormSubmit called');
        
        const taskId = taskIdInput.value;
        const task = {
            title: taskTitleInput.value,
            description: taskDescriptionInput.value,
            isCompleted: taskCompletedInput.checked
        };

        console.log('Submitting task:', task);
        console.log('Task ID:', taskId);

        if (taskId) {
            // Update existing task
            task.id = parseInt(taskId);
            console.log('Updating existing task with ID:', task.id);
            updateTask(task);
        } else {
            // Create new task
            console.log('Creating new task');
            createTask(task);
        }
    }

    function createTask(task) {
        console.log('Creating task:', task);
        fetch(apiBaseUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(task)
        })
        .then(response => {
            console.log('Create response status:', response.status);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(createdTask => {
            console.log('Task created:', createdTask);
            resetForm();
            // Add a small delay before reloading tasks to ensure the server has processed the creation
            setTimeout(loadTasks, 100);
        })
        .catch(error => {
            console.error('Error creating task:', error);
            alert('Failed to create task. Please try again.');
        });
    }

    function editTask(taskId) {
        console.log('Editing task:', taskId);
        fetch(`${apiBaseUrl}/${taskId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(task => {
                console.log('Task loaded for edit:', task);
                taskIdInput.value = task.id;
                taskTitleInput.value = task.title;
                taskDescriptionInput.value = task.description || '';
                taskCompletedInput.checked = task.isCompleted;
                formTitle.textContent = 'Edit Task';
            })
            .catch(error => {
                console.error('Error loading task for edit:', error);
                alert('Failed to load task for editing. Please try again.');
            });
    }

    function updateTask(task) {
        console.log('Updating task:', task);
        fetch(`${apiBaseUrl}/${task.id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(task)
        })
        .then(response => {
            console.log('Update response status:', response.status);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            resetForm();
            // Add a small delay before reloading tasks to ensure the server has processed the update
            setTimeout(loadTasks, 100);
        })
        .catch(error => {
            console.error('Error updating task:', error);
            alert('Failed to update task. Please try again.');
        });
    }

    function deleteTask(taskId) {
        if (confirm('Are you sure you want to delete this task?')) {
            console.log('Deleting task:', taskId);
            fetch(`${apiBaseUrl}/${taskId}`, {
                method: 'DELETE'
            })
            .then(response => {
                console.log('Delete response status:', response.status);
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                // Add a small delay before reloading tasks to ensure the server has processed the deletion
                setTimeout(loadTasks, 100);
            })
            .catch(error => {
                console.error('Error deleting task:', error);
                alert('Failed to delete task. Please try again.');
            });
        }
    }
}); 
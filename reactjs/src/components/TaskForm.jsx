import React, { useState, useEffect } from 'react';

function TaskForm({ onSubmit, editingTask, onCancel }) {
    const [title, setTitle] = useState('');

    // Update form when editing task changes
    useEffect(() => {
        if (editingTask) {
            setTitle(editingTask.title);
        } else {
            setTitle('');
        }
    }, [editingTask]);

    const handleSubmit = (e) => {
        e.preventDefault();

        if (title.trim() === '') {
            alert('Please enter a task title');
            return;
        }

        if (editingTask) {
            onSubmit(editingTask.id, { title: title.trim() });
        } else {
            onSubmit({ title: title.trim() });
        }

        setTitle('');
    };

    return (
        <form onSubmit={handleSubmit} className="task-form">
            <div className="form-group">
                <input
                    type="text"
                    value={title}
                    onChange={(e) => setTitle(e.target.value)}
                    placeholder="Enter task title..."
                    className="task-input"
                />
            </div>

            <div className="form-actions">
                <button type="submit" className="btn btn-primary">
                    {editingTask ? 'Update Task' : 'Add Task'}
                </button>

                {editingTask && (
                    <button
                        type="button"
                        onClick={onCancel}
                        className="btn btn-secondary"
                    >
                        Cancel
                    </button>
                )}
            </div>
        </form>
    );
}

export default TaskForm;
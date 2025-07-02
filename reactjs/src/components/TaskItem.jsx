import React from 'react';

function TaskItem({ task, onEdit, onDelete, onToggleComplete }) {
    return (
        <div className={`task-item ${task.completed ? 'completed' : ''}`}>
            <div className="task-content">
                <input
                    type="checkbox"
                    checked={task.completed}
                    onChange={() => onToggleComplete(task.id)}
                    className="task-checkbox"
                />
                <span className="task-title">{task.title}</span>
            </div>

            <div className="task-actions">
                <button
                    onClick={() => onEdit(task)}
                    className="btn btn-edit"
                    disabled={task.completed}
                >
                    Edit
                </button>
                <button
                    onClick={() => onDelete(task.id)}
                    className="btn btn-delete"
                >
                    Delete
                </button>
            </div>
        </div>
    );
}

export default TaskItem;
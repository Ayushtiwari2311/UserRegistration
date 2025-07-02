import React from 'react';
import TaskItem from './TaskItem';

function TaskList({ tasks, onEdit, onDelete, onToggleComplete }) {
    if (tasks.length === 0) {
        return (
            <div className="task-list">
                <p className="no-tasks">No tasks yet. Add one above!</p>
            </div>
        );
    }

    return (
        <div className="task-list">
            <h2>Tasks ({tasks.length})</h2>
            {tasks.map(task => (
                <TaskItem
                    key={task.id}
                    task={task}
                    onEdit={onEdit}
                    onDelete={onDelete}
                    onToggleComplete={onToggleComplete}
                />
            ))}
        </div>
    );
}

export default TaskList;
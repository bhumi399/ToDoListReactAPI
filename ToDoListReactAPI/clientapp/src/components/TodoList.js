import React, { useEffect, useState } from "react";
import { getUsers, getTasksByUserId } from "../services/todoService";
import { useMediaQuery } from 'react-responsive';
import './style.css';

const TodoList = () => {
    const [users, setUsers] = useState([]);
    const [tasks, setTasks] = useState([]);
    const [selectedUserId, setSelectedUserId] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [statusFilter, setStatusFilter] = useState(""); //for dropdown filter

    //detect screen size
    const isMobile = useMediaQuery({ query: '(max-width: 767px)' });
    const isTablet = useMediaQuery({ query: '(min-width: 768px) and (max-width: 1023px)' });
    const isDesktop = useMediaQuery({ query: '(min-width: 1024px)' });

    //fetch users on component load
    useEffect(() => {
        const fetchUsers = async () => {
            try {
                setLoading(true);
                const userData = await getUsers();
                setUsers(userData);
                setLoading(false);
            } catch (err) {
                setError(err.message);
                setLoading(false);
            }
        };
        fetchUsers();
    }, []);

    //fetch tasks when a user is selected
    const handleUserSelect = async (userId) => {
        setSelectedUserId(userId);
        setTasks([]);
        setError(null);
        try {
            setLoading(true);
            const taskData = await getTasksByUserId(userId);
            setTasks(Array.isArray(taskData) ? taskData : []);
            setLoading(false);
        } catch (err) {
            setError(err.message);
            setLoading(false);
        }
    };

    //Filter tasks based on selected status
    const filteredTasks = tasks.filter((task) => {
        return statusFilter ? task.status === statusFilter : true;
    });

    //function for toggle status
    const toggleStatus = async (taskId, currentStatus) => {
        const newStatus = currentStatus === "Completed" ? "Pending" : "Completed";

        try {
            // Send update request to the backend
            await fetch(`https://localhost:7083/api/Todo/UpdateTaskStatus/${taskId}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ Status: newStatus }),
            });

            console.log(`/api/Todo/UpdateTaskStatus/${taskId}`);
            // Optionally, update the UI state if the backend call is successful
            setTasks((prevTasks) =>
                prevTasks.map((task) =>
                    task.taskId === taskId ? { ...task, status: newStatus } : task
                )
            );
        } catch (error) {
            console.error("Error updating task status:", error);
        }
    }; 


    return (
        <div className={`todo-container ${isMobile ? 'mobile' : isTablet ? 'tablet' : 'desktop'}`}>
            <h1>To-Do List</h1>

            {loading && <p>Loading...</p>}
            {
                error && <p style={{ color: 'red' }}>{error}</p>
            }

            <h2>Users</h2>
            <ul>
                {users.map((user) => (
                    <li key={user.userId}>
                        <button onClick={() => handleUserSelect(user.userId)}>
                            {user.name}
                        </button>
                    </li>
                ))}
            </ul>

            {selectedUserId && (
                <>
                    <h2>Tasks for User {selectedUserId}</h2>

                    { /* To show all the tasks from selected user */}
                    { /*
                    {tasks.length > 0 ? (
                        <ul>
                            {tasks.map((task) => (
                                <li key={task.taskId}>
                                    <strong>{task.title}</strong> - {task.status}
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <p>No tasks found for this user.</p>
                    )}
                    */}

                    {tasks.length === 0 && <p>No tasks found for this user.</p>}



                    { /*dropdown for filtering tasks */}
                    {tasks.length > 0 && (
                        <label>
                            Filter by Status:
                            <select
                                value={statusFilter}
                                onChange={(e) => setStatusFilter(e.target.value)}
                            >
                                <option value="">All</option>
                                <option value="Completed">Completed</option>
                                <option value="Pending">Pending</option>
                            </select>
                        </label>
                    )}
                    
                    { /* display filtered tasks*/}
                    {filteredTasks.length > 0 ? (
                        <ul>
                            {filteredTasks.map((task) => (
                                <li key={task.taskId}>
                                    <strong>{task.title}</strong> - {task.status}
                                    { /*Toggle Button*/}
                                    <button onClick={() => toggleStatus(task.taskId, task.status)}>
                                        {task.status === "Completed"?"Mark as Pending" : "Mark as Completed"}
                                    </button>
                                </li>
                            ))}
                        </ul>
                    ) : (
                        tasks.length>0 && < p > No tasks found for this user.</p>
                    )}
                </>
            )}
        </div>
    );
};

export default TodoList;


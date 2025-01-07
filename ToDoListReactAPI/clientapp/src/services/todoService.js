const USERS_API_URL = "https://localhost:7083/api/Todo/GetAllUsers";
const TASKS_API_URL = "https://localhost:7083/api/Todo/GetAllTasksByUserId/";


//fetch users
export async function getUsers() {
    try {
        const response = await fetch(USERS_API_URL);
        if (!response.ok) {
            throw new Error(`HTTP error status message : ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Error fetching users:', error);
        throw error;
    }
}

//fetch tasks
export async function getTasksByUserId(userId) {
    try {
        const response = await fetch(`${TASKS_API_URL}${userId}`);
        if (!response.ok) {
            throw new Error(`HTTP error status message : ${response.status}`);
        }
        return await response.json();
    }
    catch (error) {
        console.error('Error fetching users:', error);
        throw error;
    }
}


import { TodoItem } from "./contracts/TodoItem"
import baseClient from './axiosClient'

class TodoItemsApiClient {
    getActive(): Promise<TodoItem[]> {
        return baseClient.get('todo-items', {
            params: {
                isCompleted: false
            }
        })
            .then((response) => response.data);        
    }

    getCompleted(): Promise<TodoItem[]> {
        return baseClient.get('todo-items', {
            params: {
                isCompleted: true
            }
        })
            .then((response) => response.data);        
    }

    delete(id: number) {
        return baseClient.delete(`todo-items/${id}`);
    }

    update(todoItem: TodoItem) {
        return baseClient.put(`todo-items/${todoItem.id}`, todoItem);
    }

    insert(todoItem: TodoItem){
        return baseClient.post('todo-items', todoItem);
    }
}

export default new TodoItemsApiClient();
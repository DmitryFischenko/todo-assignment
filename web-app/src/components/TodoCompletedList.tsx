import { Button, Divider, List, Space } from 'antd'
import TodoInput, { SubmitResult } from './TodoInput'
import todoApiClient from './../api-clients/TodoItemsApiClient';
import { TodoItem } from '../api-clients/contracts/TodoItem';
import { useEffect, useState } from 'react';
import TodoItemView from './TodoItemView';

function TodoCompletedList() {

    const [todoItemList, setTodoItemList] = useState<TodoItem[]>([]);

    const [loading, setLoading] = useState<boolean>(false)

    const fetch = async () => {
        setLoading(true);
        const result = await todoApiClient.getCompleted();
        setLoading(false);
        setTodoItemList(result);
    }

    const onDelete = async (todoItem: TodoItem)  => {
        await todoApiClient.delete(todoItem.id as number);
    
        var copy = todoItemList.slice();
    
        copy.splice(copy.indexOf(todoItem), 1);
        setTodoItemList(copy);
    }
    
    const onMarkedAsActive = async (todoItem: TodoItem) => {
        await todoApiClient.update({
          id: todoItem.id,
          title: todoItem.title,
          isCompleted: false
        })
    
        fetch();
    }

     useEffect(() => {
         fetch();
     }, [])

    return (
        <>
            <Divider orientation="left"></Divider>
            <List
                size="large"
                loading={loading}
                bordered
                dataSource={todoItemList}          
                rowKey="id"       
                renderItem={item => <List.Item>
                    <TodoItemView item={item} 
                        onDelete={onDelete} 
                        onMarkedAsCompleted={onMarkedAsActive}></TodoItemView>
                </List.Item>}
            />
        </>
    )
}

export default TodoCompletedList

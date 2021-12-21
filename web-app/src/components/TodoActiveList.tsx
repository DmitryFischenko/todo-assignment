import { Button, Divider, List, Space } from 'antd'
import TodoInput, { SubmitResult } from './TodoInput'
import todoApiClient from './../api-clients/TodoItemsApiClient';
import { TodoItem } from '../api-clients/contracts/TodoItem';
import { useEffect, useState } from 'react';
import TodoItemView from './TodoItemView';
import Item from 'antd/lib/list/Item';

function TodoActiveList() {

    const [todoItemList, setTodoItemList] = useState<TodoItem[]>([]);

    const [loading, setLoading] = useState<boolean>(false)

    const fetch = async () => {
        setLoading(true);
        const result = await todoApiClient.getActive();
        setLoading(false);
        setTodoItemList(result);
    }

    const onDelete = async (todoItem: TodoItem)  => {
        await todoApiClient.delete(todoItem.id as number);
    
        var copy = todoItemList.slice();
    
        copy.splice(copy.indexOf(todoItem), 1);
        setTodoItemList(copy);
    }
    
    const onMarkedAsCompleted = async (todoItem: TodoItem) => {
        await todoApiClient.update({
          id: todoItem.id,
          title: todoItem.title,
          isCompleted: true
        })
    
        fetch();
    }

    const onNewValueSubmit = async (value: string): Promise<SubmitResult> => {    
        await todoApiClient.insert({
          "id": 0,
          "title": value,
          "isCompleted": false
        })
 
        fetch();

       return {
         isSuccesful: true
       }
     }  

     useEffect(() => {
         fetch();
     }, [])

    return (
        <>
            <TodoInput onSubmit={onNewValueSubmit} placeHolderText='Type ToDo item title' />

            <Divider orientation="left"></Divider>
            <List
                loading = {loading}
                size="large"
                bordered
                dataSource={todoItemList}             
                rowKey="id" 
                renderItem={item => <List.Item>
                    <TodoItemView item={item} 
                        onDelete={onDelete} 
                        onMarkedAsCompleted={onMarkedAsCompleted}></TodoItemView>
                </List.Item>}
            />
        </>
    )
}

export default TodoActiveList

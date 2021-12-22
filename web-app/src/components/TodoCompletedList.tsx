import { Divider, List } from 'antd'
import todoApiClient from './../api-clients/TodoItemsApiClient';
import { TodoItem } from '../api-clients/contracts/TodoItem';
import { useEffect, useState } from 'react';
import TodoItemView from './TodoItemView';
import _ from 'lodash';

function TodoCompletedList() {

    const [todoItemList, setTodoItemList] = useState<TodoItem[]>([]);

    const [loading, setLoading] = useState<boolean>(false)

    const fetch = () => {

        setLoading(true);

        todoApiClient.getCompleted().then((items) => {
            setTodoItemList(items);
            setLoading(false);
        });        
    }

    const removeFromStateById = (id: number) => {
        let newList =_.clone(todoItemList);
        _.remove(newList, {id: id});
        setTodoItemList(newList);
    }

    const onDelete = (todoItem: TodoItem)  => {
        todoApiClient.delete(todoItem.id).then(() => {
            removeFromStateById(todoItem.id);
        });
    }
    
    const onMarkedAsActive = (todoItem: TodoItem) => {
        todoApiClient.update({
          id: todoItem.id,
          title: todoItem.title,
          isCompleted: false
        })

        removeFromStateById(todoItem.id);
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
                        onMarkedAsCompleted={onMarkedAsActive}
                        onEdit = {() => {}}
                        ></TodoItemView>
                </List.Item>}
            />
        </>
    )
}

export default TodoCompletedList

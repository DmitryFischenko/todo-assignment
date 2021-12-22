import { Divider, Form, Input, List, Modal } from 'antd'
import todoApiClient from './../api-clients/TodoItemsApiClient';
import { TodoItem } from '../api-clients/contracts/TodoItem';
import { useEffect, useState } from 'react';
import TodoItemView from './TodoItemView';
import _ from "lodash";
import strings from './../localization';

function TodoActiveList() {

    const [form] = Form.useForm();

    const [modalForm] = Form.useForm();

    const [todoItemList, setTodoItemList] = useState<TodoItem[]>([]);

    const [itemForEdit, setItemForEdit] = useState<TodoItem>();

    const [loading, setLoading] = useState<boolean>(false)

    const [visible, setVisible] = useState(false);

    const fetch = () => {

        setLoading(true);

        todoApiClient.getActive().then((items) => {
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
    
    const onMarkedAsCompleted = (todoItem: TodoItem) => {
        todoApiClient.update({
          id: todoItem.id,
          title: todoItem.title,
          isCompleted: true
        })

        removeFromStateById(todoItem.id);
    }

    const onAdd = ({title}: any) => {    

        return todoApiClient.insert({
          "id": 0,
          "title": title,
          "isCompleted": false
        }).then((insertedItem) => {
           let newList = _.clone(todoItemList);
           newList.push(insertedItem);
           setTodoItemList(newList);
           form.resetFields();
        })
     }  
     
    const showModal = (item: TodoItem) => {

        setItemForEdit(item);

        modalForm.setFieldsValue({
            title: item.title
        })

        setVisible(true);
    };

    const handleOk = () => {

        let newTitle = modalForm.getFieldValue('title');

        if (!itemForEdit)
            return;

        todoApiClient.update({
            id: itemForEdit.id,
            title: newTitle,
            isCompleted: itemForEdit.isCompleted
        }).then((updatedItem) => {

            let clonedList = _.clone(todoItemList);
            let index = _.findIndex(clonedList, {id: updatedItem.id});
            clonedList.splice(index, 1, updatedItem);

            setTodoItemList(clonedList);

            setVisible(false);
            }               
        )
    };

    const handleCancel = () => {
        setVisible(false);
    };

     useEffect(() => {
         fetch();
     }, [])

    return (
        <>
           <Form onFinish={onAdd} form={form}>
             <Form.Item 
                 name="title"  rules={[{ required: true, message: strings.input_error_empty }]}>
                 <Input                     
                    placeholder={strings.newItemPrompt} 
                    maxLength={100}
                    size="large"  />  
                </Form.Item>
            </Form>

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
                        onMarkedAsCompleted={onMarkedAsCompleted}
                        onEdit={showModal}></TodoItemView>
                </List.Item>}
            />

            <Modal
                title="Update Todo item text"
                visible={visible}
                onOk={handleOk}
                onCancel={handleCancel}>

                <Form onFinish={handleOk} form={modalForm}>
                    <Form.Item name="title" rules={[{ required: true, message: 'Please provide Todo item text' }]} initialValue={itemForEdit?.title} >
                        <Input
                            maxLength={100}
                            size="large" />
                    </Form.Item>
                </Form>
            </Modal>
        </>
    )
}

export default TodoActiveList

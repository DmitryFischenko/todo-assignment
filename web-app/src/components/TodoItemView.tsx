import { Button, Form, Input, Modal } from 'antd'
import Space from 'antd/lib/space'
import CheckOutlined from '@ant-design/icons/lib/icons/CheckOutlined';
import UndoOutlined from '@ant-design/icons/lib/icons/UndoOutlined';
import ExclamationCircleOutlined from '@ant-design/icons/lib/icons/ExclamationCircleOutlined';
import DeleteOutlined from '@ant-design/icons/lib/icons/DeleteOutlined';
import EditOutlined from '@ant-design/icons/lib/icons/EditOutlined';

import { TodoItem } from '../api-clients/contracts/TodoItem';

import strings from '../localization'

interface TodoItemProps {
    item: TodoItem;
    onDelete(item: TodoItem): void;
    onMarkedAsCompleted(item: TodoItem): void;
    onEdit(item: TodoItem): void;
}

function TodoItemView(props: TodoItemProps) {

    const { confirm } = Modal;

    const showDeleteConfirm = () => {
        confirm({
          title: strings.deleteConfirmation,
          icon: <ExclamationCircleOutlined />,
          content: strings.deleteConfirmationDesc,
          okText: strings.yes,
          okType: 'danger',
          cancelText: strings.no,
          onOk() {
            props.onDelete(props.item)
          },
        } );
    }

    return (
        <>
            <Space>
                <Button  style={{color: 'light-blue'}} icon={props.item.isCompleted ? <UndoOutlined /> : <CheckOutlined />}  onClick={e => props.onMarkedAsCompleted(props.item)} />
                {props.item.title}
            </Space>
            <Space>                
                 {!props.item.isCompleted ? <Button onClick={e => props.onEdit(props.item)} type='primary' icon={<EditOutlined />} /> : null }
                 <Button danger style={{ float: 'right' }} onClick={e => showDeleteConfirm()} icon={<DeleteOutlined />} />
            </Space>
        </>
    )
}

export default TodoItemView

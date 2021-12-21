import { Button } from 'antd'
import Space from 'antd/lib/space'
import CheckOutlined from '@ant-design/icons/lib/icons/CheckOutlined';
import DeleteOutlined from '@ant-design/icons/lib/icons/DeleteOutlined';
import { TodoItem } from '../api-clients/contracts/TodoItem';

interface TodoItemProps {
    item: TodoItem;
    onDelete(item: TodoItem): void;
    onMarkedAsCompleted(item: TodoItem): void;
}

function TodoItemView(props: TodoItemProps) {
    return (
        <>
            <Space>
                <Button style={{color: 'green'}} icon={<CheckOutlined />} onClick={e => props.onMarkedAsCompleted(props.item)} />
                {props.item.title}
            </Space>
            <Button danger style={{ float: 'right' }} onClick={e => props.onDelete(props.item)} icon={<DeleteOutlined />} />
        </>
    )
}

export default TodoItemView

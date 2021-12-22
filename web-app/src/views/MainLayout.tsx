import { Button, Divider, Layout, PageHeader, Space } from 'antd'
import { Content } from 'antd/lib/layout/layout'
import { Route, Routes } from 'react-router'
import { Link } from 'react-router-dom'
import TodoActiveList from '../components/TodoActiveList'
import TodoCompletedList from '../components/TodoCompletedList'
import strings from '../localization'

import { useLocation } from 'react-router-dom';

enum Views {
    Active,
    Completed
}

function MainLayout() {   

    const viewType = useLocation().pathname == '/completed' ? Views.Completed : Views.Active;

    return (
        <>
        <Layout style={{ maxWidth: '1280px', margin: 'auto'}}>
        <Content >
          <PageHeader
            title={ strings.title } >
              <Space>
                <Button type={viewType == Views.Active ? "primary": "default"} ><Link to="/active">{strings.lbl_activeTasks}</Link></Button>
                <Button type={viewType == Views.Completed? "primary": "default"} ><Link to="/completed">{strings.lbl_completedTasks}</Link></Button>
              </Space>
              <Divider/>
            <Routes>
              <Route path="/active" element={<TodoActiveList />} />
              <Route path="/completed" element={<TodoCompletedList />} />
              <Route path="*" element={<TodoActiveList/>} />
            </Routes>

          </PageHeader></Content>
      </Layout>
      </>
    )
}

export default MainLayout

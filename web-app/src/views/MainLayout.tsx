import { Button, Layout, PageHeader } from 'antd'
import { Content } from 'antd/lib/layout/layout'
import { Route, Routes } from 'react-router'
import { Link } from 'react-router-dom'
import TodoActiveList from '../components/TodoActiveList'
import TodoCompletedList from '../components/TodoCompletedList'

import { useLocation } from 'react-router-dom';
import { useEffect, useState } from 'react'

enum Views {
    Active,
    Completed
}

function MainLayout() {
    
    const [title, setTitle] = useState("Todo Manager");

    const viewType = useLocation().pathname == '/active' ? Views.Active : Views.Completed;

    useEffect(() => {
        setTitle(viewType == Views.Active ? "Todo Manager - Active Tasks" : "Todo Manager - Completed Tasks")
    }, [viewType])


    return (
        <>
        <Layout  style={{ margin: '0 50px'}}>
        <Content>
          <PageHeader
            title={ title }
            extra={[
              <Button type={viewType == Views.Active ? "primary": "default"} ><Link to="/active">Active Tasks</Link></Button>,
              <Button type={viewType == Views.Completed? "primary": "default"} ><Link to="/completed">Completed Tasks</Link></Button>
            ]}
          >

            <Routes>
              <Route path="/active" element={<TodoActiveList />} />
              <Route path="/completed" element={<TodoCompletedList />} />
            </Routes>

          </PageHeader></Content>
      </Layout>
      </>
    )
}

export default MainLayout

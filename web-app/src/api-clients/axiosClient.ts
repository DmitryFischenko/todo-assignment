import { notification } from 'antd';
import axios from 'axios';

const openNotification = (error:string) => {
    notification.error({
      message: `Error`,
      description: error,
    });
  };

const axiosClient = axios.create();

axiosClient.defaults.baseURL = "https://localhost:5001/api"

axiosClient.interceptors.response.use(function (config) {
    return config;
  }, function (error) {
    openNotification(error.response.data.message);
    return Promise.reject(error);
  });


export default axiosClient;


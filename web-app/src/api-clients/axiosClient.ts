import axios from 'axios';

const axiosClient = axios.create();

axiosClient.defaults.baseURL = "https://localhost:5001"

export default axiosClient;


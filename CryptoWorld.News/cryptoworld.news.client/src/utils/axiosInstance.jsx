import axios from "axios";

const axiosInstance = axios.create({
  baseURL: "https://localhost:7249",
});

export default axiosInstance;

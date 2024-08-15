import axios from "axios";

const axiosInstance = axios.create({
  baseURL: "https://localhost:7249",
  headers: {
    'Content-Type': 'application/json'
  },
});

axiosInstance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers['Authorization'] = 'Bearer ' + token;
    }
    return config;
  },
  (error) => {
    console.log(error);
    return Promise.reject(error);
  }
);

axiosInstance.interceptors.response.use(
  (res) => {
    return res;
  },
  async (err) => {
    const originalConfig = err.config;

    if (originalConfig.url !== "/account/login" && err.response) {
      if (err.response.status === 401 && !originalConfig._retry) {
        originalConfig._retry = true;
        const token = localStorage.getItem('token');
        const refreshToken = localStorage.getItem('refreshToken');
        try {
          const response = await axios.post('https://localhost:7249/Account/refresh-token', {
            token: token,
            refreshToken: refreshToken
          }, {
            headers: {
              'Content-Type': 'application/json'
            }
          });

          const newToken = response.data.token;
          const newRefreshToken = response.data.refreshToken
          localStorage.token = newToken;
          localStorage.refreshToken = newRefreshToken;

          return axiosInstance(originalConfig);
        } catch (_error) {
          return Promise.reject(_error);
        }
      }
    }
    console.log(err);
    return Promise.reject(err);
  }
)

export default axiosInstance;

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
      //  config.headers['www-authenticate'] = token;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

axiosInstance.interceptors.response.use(
  (res) => {
    return res;
  },
  async (err) => {
    const originalConfig = err.config;

    if (err.response) {
      if (err.response.status === 401) {
        const token = localStorage.getItem('token');
        const refreshToken = localStorage.getItem('refreshToken');
        try {
          const requestTokens = await axios.post('https://localhost:7249/Account/refresh-token', {
            token: token,
            refreshToken: refreshToken
          }, {
            headers: {
              'Content-Type': 'application/json'
            }
          });

          const newToken = requestTokens.token;
          const newRefreshToken = requestTokens.refreshToken
          localStorage.token = newToken;
          localStorage.refreshToken = newRefreshToken;

          return axiosInstance(originalConfig);
        } catch (_error) {
          return Promise.reject(_error);
        }
      }
    }
    return Promise.reject(err);
  }
)

export default axiosInstance;

﻿import axios from 'axios';
import URL from '../url';
import store from '../store';

const instance = axios.create({
    baseURL: URL.apiUrl
});

// 全域設定 AJAX Request 攔截器 (interceptor)
instance.interceptors.request.use((config) => {
    const token = store.getters.getAccessToken;

    // Optional headers
    if (token) {
        //console.log("set token");
        config.headers.Authorization = `Bearer ${token}`;
    }
    /*
    console.log(JSON.stringify(config));
    console.log(config.headers.Authorization);
    console.log(config.baseURL + config.url);
    console.log(config.method);
    */
    return config;

}, (error) => {
    console.log(JSON.stringify(error));
    return Promise.reject(error);
});


// 全域設定 AJAX Response 攔截器 (interceptor)
instance.interceptors.response.use((response) => {
    return response
}, (error) => {
    console.log(JSON.stringify(error));
    if (!error.response) {
        return Promise.reject(error)
    }
    if (error.response) {
        // server responded status code falls out of the range of 2xx
        switch (error.response.status) {
            case 400:
                {
                    const { message } = error.response.data
                    alert(`${error.response.status}: ${message || '資料錯誤'}。`)
                }

                break

            case 401:
                {

                    // 當不是 refresh token 作業發生 401 才需要更新 access token 並重發
                    // 如果是就略過此刷新 access token 作業，直接不處理(因為 catch 已經攔截處理更新失敗的情況了)
                    const refreshTokeUrl = `${URL.apiUrl}/api/auth/RefreshToken`
                    if (error.config.url !== refreshTokeUrl) {
                        // 原始 request 資訊
                        const originalRequest = error.config
                        // 依據 refresh_token 刷新 access_token 並重發 request
                        return axios
                            .post(`${refreshTokeUrl}?refreshToken=${store.getters.getRefreshToken}`) // refresh_toke is attached in cookie
                            .then((response) => {
                                // [更新 access_token 成功]
                                //console.log(JSON.stringify(response.data));

                                // 刷新 storage (其他呼叫 api 的地方都會從此處取得新 access_token)
                                store.dispatch("updateToken", response.data.data);

                                // 刷新原始 request 的 access_token
                                originalRequest.headers.Authorization = 'Bearer ' + store.getters.getAccessToken

                                // 重送 request (with new access_token)
                                return axios(originalRequest)
                            })
                            .catch((err) => {
                                // [更新 access_token 失敗] ( e.g. refresh_token 過期無效)
                                store.token.value = ''
                                alert(`${err.response.status}: 作業逾時或無相關使用授權，請重新登入`)
                                window.location.href = '/login'
                                return Promise.reject(error)
                            })
                    }
                }

                break

            case 404:
                alert(`${error.response.status}: 資料來源不存在`)
                break

            case 500:
                alert(`${error.response.status}: 內部系統發生錯誤`)
                break

            default:
                alert(`${error.response.status}: 系統維護中，造成您的不便，敬請見諒。`)

                break
        }
    } else {
        // Something happened in setting up the request that triggered an Error
        if (error.code === 'ECONNABORTED' && error.message && error.message.indexOf('timeout') !== -1) {
            // request time out will be here
            alert('網路連線逾時，請點「確認」鍵後繼續使用。')
        } else {
            // shutdonw api server
            alert('網路連線不穩定，請稍候再試')
        }
    }

    return Promise.reject(error)
});

export default instance;
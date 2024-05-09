import { useEffect } from "react";
import useAuth from "./useAuth";
import { axiosPrivate } from "../axios/axios";

const useAxiosPrivate = () => {
    //const refresh = useRefreshToken();
    const [auth] = useAuth();
    useEffect(() => {
        const requestIntercep = axiosPrivate.interceptors.request.use(
            config=> {
                if(!config.headers['Authorization']) {
                    config.headers['Authorization'] = `Bearer ${auth?.access_token}`;
                }
                return config;
            }, (error) => Promise.reject(error)
        )
        // const responseIntercep = axiosPrivate.interceptors.response.use(
        //     response => response,
        //     async (error) => {
        //         const prevRequest = error?.config;
        //         if(error?.response?.status === 403 && !prevRequest?.sent) {
        //             prevRequest.sent = true;
        //             const newAccessToken = await refresh();
        //             prevRequest.headers['Authorization'] = `Bearer ${newAccessToken}`;
        //             return axiosPrivate(prevRequest);
        //         }
        //         return Promise.reject(error);
        //     }
        // );
        return () => {
            axiosPrivate.interceptors.request.eject(requestIntercep);
            //axiosPrivate.interceptors.response.eject(responseIntercep);
        }
    }, [auth])   
    return axiosPrivate;
}
export default useAxiosPrivate;
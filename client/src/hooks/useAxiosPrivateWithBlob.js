import { useEffect } from "react";
import useAuth from "./useAuth";
import { axiosPrivateForFile } from "../axios/axios";

const useAxiosPrivateWithBlob = () => {
    //const refresh = useRefreshToken();
    const [auth] = useAuth();
    useEffect(() => {
        const requestIntercep = axiosPrivateForFile.interceptors.request.use(
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
            axiosPrivateForFile.interceptors.request.eject(requestIntercep);
            //axiosPrivate.interceptors.response.eject(responseIntercep);
        }
    }, [auth])   
    return axiosPrivateForFile;
}
export default useAxiosPrivateWithBlob;
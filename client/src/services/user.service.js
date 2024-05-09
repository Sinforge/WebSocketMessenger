import axios from "../axios/axios"

export function registerUser(data) {
    return axios.post("user/registration", data)
}

export function authorizeUser(data) {
    return axios.post("user/authorize", data)
}

export function findUserByUserName(userName) {
    return axios.get("user/" + userName);
}
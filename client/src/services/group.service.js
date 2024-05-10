export function createGroup(axios, groupName) {
    return axios.post("/group", {"Name": groupName})
}
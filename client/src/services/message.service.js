
export function getUserConversation(axios, userId) {
    let endpoint = "/message/conversation/" + userId;
    return axios.get(endpoint);
}

export function getUserDialogs(axios) {
    return axios.get("/message/conversation");
}

export function getUserGroups(axios) {
    return axios.get("/message/group")
}

export function getUserGroupMessages(axios, groupId) {
    let endpoint = "/message/group/"  + groupId;
    return axios.get(endpoint)
}

export function getMessageFile(axios, messageId) {
    let endpoint = "/message/file/" + messageId;
    return axios.get(endpoint)
}

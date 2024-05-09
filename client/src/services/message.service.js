
export function getUserConversation(axios, userId) {
    let endpoint = "/message/conversation/" + userId;
    return axios.get(endpoint);
}

export function getUserDialogs(axios) {
    return axios.get("/message/conversation");
}

export function getMessageFile(axios, messageId) {
    let endpoint = "/message/file/" + messageId;
    return axios.get(endpoint)
}

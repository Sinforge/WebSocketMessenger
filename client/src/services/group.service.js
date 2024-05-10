export function createGroup(axios, groupName) {
    return axios.post("/group", {"Name": groupName})
}

export function getUsersToInvite(axios, groupId, searchString) {
    return axios.post("/group/invite", {"Id": groupId, "SearchString": searchString})
}

export function addUsersToGroup(axios, groupId, ids) {
    return axios.post("/group/invite/users", {
        "Ids": ids,
        "GroupId": groupId
    })
}

export function getGroupMembers(axios, groupId) {
    var endpoint = "/group/members"
    return axios.post(endpoint, {"Id": groupId});
}
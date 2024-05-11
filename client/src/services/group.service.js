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

export function deleteUserFromGroup(axios, groupId, userId) {
    return axios.delete("/group/" + groupId +"/members/" + userId);
}

export function updateUserGroupRole(axios, groupId, userId, roleId) {
    return axios.put("/group/members", {"GroupId": groupId, "UserId": userId, "RoleId": roleId})
}
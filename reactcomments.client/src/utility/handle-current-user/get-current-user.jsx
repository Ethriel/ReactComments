import { getLocalStorageValue } from "../handle-local-storage/get-local-storage-value";
import { CURRENT_USER_ID, CURRENT_USER_EMAIL, CURRENT_USER_USERNAME, CURRENT_USER_ROLE, IS_ISGNED_IN } from '../handle-local-storage/local-storage-keys'

export const getCurrentUser = () => {
    const email = getLocalStorageValue(CURRENT_USER_EMAIL);
    return email ? {
        id: Number.parseInt(getLocalStorageValue(CURRENT_USER_ID), 10),
        email: email,
        userName: getLocalStorageValue(CURRENT_USER_USERNAME),
        role: getLocalStorageValue(CURRENT_USER_ROLE),
        isSignedIn: Boolean(getLocalStorageValue(IS_ISGNED_IN))
    } : null
}
import { setLocalStorageValue } from "../handle-local-storage/set-local-storage-value";
import { CURRENT_USER_ID, CURRENT_USER_EMAIL, CURRENT_USER_USERNAME, CURRENT_USER_ROLE, IS_ISGNED_IN } from '../handle-local-storage/local-storage-keys'

export const setCurrentUser = (userData) => {
    setLocalStorageValue(CURRENT_USER_ID, userData.id);
    setLocalStorageValue(CURRENT_USER_EMAIL, userData.email);
    setLocalStorageValue(CURRENT_USER_USERNAME, userData.userName);
    setLocalStorageValue(CURRENT_USER_ROLE, userData.role);
    setLocalStorageValue(IS_ISGNED_IN, true);
}
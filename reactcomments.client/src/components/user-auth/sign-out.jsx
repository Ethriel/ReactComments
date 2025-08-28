import { useEffect } from "react";
import { useNavigate } from "react-router-dom"

import axios from "axios";

import { getCurrentUser } from "../../utility/handle-current-user/get-current-user"
import { clearLocalStorage } from '../../utility/handle-local-storage/clear-local-storage'
import { MAIN } from "../../utility/routes/app-paths";

export const SignOut = ({ onSignOut, ...props }) => {
    const navigate = useNavigate();
    const currentUser = getCurrentUser();

    const signOutFunc = () => {
        const data = {
            email: currentUser.email
        }
        axios.post('/api/auth/sign-out', data, {
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function (response) {
                navigate(MAIN);
                clearLocalStorage();
                onSignOut?.(getCurrentUser());
            })
            .catch(function (error) {
                console.error('ERROR', error);
            });
    }

    useEffect(() => {
        signOutFunc();
    }, [])
}
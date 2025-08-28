import { Link, useNavigate } from "react-router-dom";
import axios from 'axios';
import { Typography, notification } from "antd";

import { CommonForm } from "./common/common-form";
import { commonFormStyles } from "./common/styles";
import { setCurrentUser } from "../../utility/handle-current-user/set-current-user";
import { getCurrentUser } from "../../utility/handle-current-user/get-current-user";
import { MAIN } from "../../utility/routes/app-paths";

const { Text } = Typography;

export const SignInForm = ({ onSignIn, ...props }) => {
    const navigate = useNavigate();
    const [api, contextHolder] = notification.useNotification();

    const openErrorNotification = (errorObj) => {
        const errors = errorObj.length ? errorObj : errorObj.errors;
        api['error']({
            message: 'Sign in failed',
            description: errors.toString()
                .split(",")
                .join("\n")
        })
    }

    const onFinish = (values) => {
        console.log("Received values of form: ", values);
        axios.post('api/auth/sign-in', values, {
            headers: {
                'Content-Type': 'application/json',
                'X-Captcha-Token': values.captchaToken
            }
        })
            .then((response) => {
                const userData = response.data;
                setCurrentUser(userData);
                onSignIn?.(getCurrentUser());
                navigate(MAIN);
            })
            .catch((error) => {
                console.error('ERROR', error);
                openErrorNotification(error.response.data);
            });
        // captchaRef.current?.reset();
        // form.resetFields();
    };
    const styles = commonFormStyles();
    const signInRules = {
        email: [
            {
                type: "email",
                required: true,
                message: "Please input your Email!",
            }
        ],
        password: [
            {
                required: true,
                message: "Please input your Password!",
            }
        ]
    }
    const bottomPart = <div style={styles.signup}>
        <Text style={styles.text}>Don't have an account?</Text>{" "}
        <Link to="/sign-up">
            Sign up
        </Link>
    </div>
    const signUpformProps = {
        formName: "normal_signin",
        title: "Sign In",
        isSignUpForm: false,
        rules: signInRules,
        bottomPart: bottomPart,
        onFinish: onFinish
    }

    return (
        <>
            {contextHolder}
            <CommonForm customFormProps={signUpformProps} />
        </>

    )
};
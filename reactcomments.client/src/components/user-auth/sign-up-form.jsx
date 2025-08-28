import { Link } from "react-router-dom";
import axios from 'axios';
import { Typography } from "antd";

import { CommonForm } from "./common/common-form";
import { commonFormStyles } from "./common/styles";

const { Text } = Typography;

export const SignUpForm = () => {
  const onFinish = (values) => {
    console.log("Received values of form: ", values);
    axios.post('api/auth/sign-up', values, {
      headers: {
        'Content-Type': 'application/json',
        'X-Captcha-Token': values.captchaToken
      }
    })
      .then(function (response) {
        console.log('RESPONSE', response);
      })
      .catch(function (error) {
        console.error('ERROR', error);
      });
    // captchaRef.current?.reset();
    // form.resetFields();
  };
  const styles = commonFormStyles();
  const userNameRegExp = new RegExp("^[a-zA-Z0-9_]+$");
  const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?""{}|<>_\-+=;:'\\[\]/~`]).+$/;
  const signUpRules = {
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
      },
      {
        pattern: passwordRegex,
        message: "Password must contain at least one uppercase letter, one lowercase letter, one number, one special character, and to be at least 8 characters long!"
      }
    ],
    userName:
      [
        {
          required: true,
          message: "Please input your Username!"
        },
        {
          pattern: userNameRegExp,
          message: "Userame must contain only English letters, numbers or underscores!"
        }
      ]
  }
  const bottomPart = <div style={styles.signup}>
    <Text style={styles.text}>Already have an account?</Text>{" "}
      <Link to="/sign-in">
        Sign in
      </Link>
  </div>
  const signUpformProps = {
    formName: "normal_signup",
    title: "Sign Up",
    isSignUpForm: true,
    rules: signUpRules,
    bottomPart: bottomPart,
    onFinish: onFinish
  }

  return (
    <CommonForm customFormProps={signUpformProps} />
  )
};
import { useRef } from "react";

import { Button, Form, Input, Typography } from "antd";
import { LockOutlined, MailOutlined, UserOutlined } from "@ant-design/icons";


import { commonFormStyles } from "./styles";
import { CaptchaInput } from '../../utility/CaptchaInput';

const { Title } = Typography;

export const CommonForm = ({ customFormProps, ...props }) => {
    const [form] = Form.useForm();
    const captchaRef = useRef(null);

    const styles = commonFormStyles();

    return (
        <section style={styles.section}>
            <div style={styles.container}>
                <div style={styles.header}>
                    <svg
                        width="33"
                        height="32"
                        viewBox="0 0 33 32"
                        fill="none"
                        xmlns="http://www.w3.org/2000/svg"
                    >
                        <rect x="0.125" width="32" height="32" rx="6.4" fill="#1890FF" />
                        <path
                            d="M19.3251 4.80005H27.3251V12.8H19.3251V4.80005Z"
                            fill="white"
                        />
                        <path d="M12.925 12.8H19.3251V19.2H12.925V12.8Z" fill="white" />
                        <path d="M4.92505 17.6H14.525V27.2001H4.92505V17.6Z" fill="white" />
                    </svg>
                    <Title style={styles.title}>{customFormProps.title}</Title>
                </div>
                <Form
                    form={form}
                    name={customFormProps.formName}
                    onFinish={customFormProps.onFinish}
                    layout="vertical"
                    requiredMark="optional"
                >
                    {
                        customFormProps.isSignUpForm === true &&
                        <Form.Item
                            name="userName"
                            rules={customFormProps.rules.userName}
                        >
                            <Input prefix={<UserOutlined />} placeholder="Userame" />
                        </Form.Item>
                    }

                    <Form.Item
                        name="email"
                        rules={customFormProps.rules.email}
                    >
                        <Input prefix={<MailOutlined />} placeholder="Email" />
                    </Form.Item>
                    <Form.Item
                        name="password"
                        // extra="Password needs to be at least 8 characters."
                        rules={customFormProps.rules.password}
                    >
                        <Input.Password
                            prefix={<LockOutlined />}
                            type="password"
                            placeholder="Password"
                        />
                    </Form.Item>
                    <Form.Item
                        name="captchaToken"
                        rules={[
                            {
                                required: true,
                                message: "Complete CAPTCHA"
                            }
                        ]}>
                        <CaptchaInput style={{ margin: '0 auto' }} ref={captchaRef} />
                    </Form.Item>
                    <Form.Item style={{ marginBottom: "0px" }}>
                        <Button block type="primary" htmlType="submit">
                            {customFormProps.title}
                        </Button>
                    </Form.Item>
                    {
                        customFormProps.bottomPart
                    }
                </Form>
            </div>
        </section >
    );
}
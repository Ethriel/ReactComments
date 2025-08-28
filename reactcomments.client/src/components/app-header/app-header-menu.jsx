import { Link } from "react-router-dom";
import { Menu, Button } from "antd";

import { ADD_COMMENT, MAIN, SIGN_IN, SIGN_OUT, SIGN_UP } from "../../utility/routes/app-paths";
import { getCurrentUser } from "../../utility/handle-current-user/get-current-user";
export const AppHeaderMenu = ({currentUser, ...props }) => {

    currentUser = getCurrentUser();
    const isSignedIn = currentUser === null ? false : currentUser.isSignedIn;

    const subItems = [
        {
            key: MAIN,
            text: "Home",
            to: MAIN,
            disabled: false
        },
        {
            key: SIGN_IN,
            text: "Sign In",
            to: SIGN_IN,
            disabled: false
        },
        {
            key: SIGN_UP,
            text: "Sign Up",
            to: SIGN_UP,
            disabled: false
        },
        {
            key: ADD_COMMENT,
            text: isSignedIn ? "Add Comment" : "Sign in to add a comment",
            to: ADD_COMMENT,
            disabled: !isSignedIn
        },
        {
            key: SIGN_OUT,
            text: "Sign out",
            to: SIGN_OUT,
            disabled: !isSignedIn
        }
    ];

    const menuItems = subItems.map((item) => {
        return <Menu.Item
            key={item.key}>
            <Button
                key={item.key}
                type='default'
                disabled={item.disabled}>
                <Link
                    key={item.key}
                    to={item.to} />
                {item.text}
            </Button>
        </Menu.Item>;
    });

    return (
        <Menu
            mode="horizontal"
            style={{ width: '100%', justifyContent: 'center' }}>
            {menuItems}
        </Menu>
    )
}
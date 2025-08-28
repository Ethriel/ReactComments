import { useState } from 'react'
import { Routes, Route } from 'react-router-dom';

import { Layout, theme } from 'antd';

import { SIGN_UP, MAIN, SIGN_IN, ADD_COMMENT, COMMENT_DETAILS, SIGN_OUT, ADD_REPLY } from './utility/routes/app-paths';
import { AppHeaderMenu } from './components/app-header/app-header-menu';
import { SignUpForm } from './components/user-auth/sign-up-form';
import { SignInForm } from './components/user-auth/sign-in';
import { SignOut } from './components/user-auth/sign-out';
import { MainComponent } from './components/main/main-component';
import { AddComment } from './components/comment/add-comment';
import { CommentDetails } from './components/comment/comment-details';
import { getCurrentUser } from './utility/handle-current-user/get-current-user';
import './App.css';

const { Header, Content, Footer } = Layout;

function App() {
    const {
        token: { colorBgContainer, borderRadiusLG },
    } = theme.useToken();
    const [currentUser, setCurrentUser] = useState(getCurrentUser());
    return (
        <Layout style={{ height: '100%' }}>
            <Header style={{ display: 'flex', alignItems: 'center' }}>
                {/* <div className="demo-logo" /> */}
                <AppHeaderMenu currentUser={currentUser} />
            </Header>
            <Content style={{ padding: '0 48px', flexWrap: 'wrap' }}>
                <Routes>
                    <Route path={MAIN} Component={MainComponent} />
                    <Route path={SIGN_UP} Component={SignUpForm} />
                    <Route path={SIGN_IN} element={<SignInForm onSignIn={setCurrentUser}/>} />
                    <Route path={SIGN_OUT} element={<SignOut onSignOut={setCurrentUser}/>} />
                    <Route path={ADD_COMMENT} Component={AddComment} />
                    <Route path={ADD_REPLY} Component={AddComment} />
                    <Route path={COMMENT_DETAILS} Component={CommentDetails} />
                </Routes>
            </Content>
            <Footer style={{ textAlign: 'center' }}>
                React comments app ©{new Date().getFullYear()} Created by Ethriel
            </Footer>
        </Layout>
    );
}

export default App;
import { useEffect, useState } from 'react';
import { signUp, main } from './utility/routes/app-paths';
import { SignUpForm } from './components/sign-up/sign-up-form'
import { Routes, Route } from 'react-router-dom';
import './App.css';

function App() {
    return (
        <Routes>
            <Route path={main} Component={SignUpForm} />
            {/* <Route path='/transfer' Component={transfer}></Route>
            <Route path='/withdrow' Component={withdrow}></Route>
            <Route path='/deposit' Component={deposit}></Route>
            <Route path='/create' Component={create}></Route> */}
        </Routes>
    );
}

export default App;
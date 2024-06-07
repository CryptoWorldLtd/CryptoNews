import React, { useState } from 'react';
import { useNavigate } from "react-router-dom"
import './Login.css'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser, faEnvelope } from '@fortawesome/free-regular-svg-icons';
import { faLock } from '@fortawesome/free-solid-svg-icons';
import axios from "axios";

const Login = () => {
    const[values,setValues] = useState({
        username: "",
        password: "",
        showPassword: false
    });
    const handlePasswordVisibility = () =>{
        setValues({
            ...values,
            showPassword: !values.showPassword
        });
    }
    console.log(values);
    return (
        <div className="container">
            <div className="header">
                <div className="text">Welcome Back</div>
                <div className="underline"></div>
            </div>
            <div className="inputs">
                <div className="input">
                <i>
                <FontAwesomeIcon icon={faUser} />
              </i>
              <input 
                type='text' 
                placeholder='Name' 
                onChange={(e)=>setValues({...values,username:e.target.value})} 
              />
                </div>
            </div>
            <div className="inputs">
                <div className="input">
                <i>
            <FontAwesomeIcon icon={faLock} />
          </i>
          <input 
                type={values.showPassword ? "text" : "password"} 
                placeholder='Password' 
                onChange={(e)=>setValues({...values,password:e.target.value})} 
              />
                </div>
            </div>
            <div className="forgot-password">Forgot Password? <span>Click Here!</span></div>
            <div className="submit-container">
                <div className="submit">Sign Up</div>
                <div className="submit">Login</div>
            </div>
        </div>
    )
};
export default Login
import React, { useState } from 'react';
import { useNavigate } from "react-router-dom";
import { NavLink as Link } from 'react-router-dom';
import axios from 'axios';
import {  faEnvelope } from '@fortawesome/free-regular-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser, faLock , faEye, faEyeSlash} from '@fortawesome/free-solid-svg-icons';
import './Login.module.css';

const Login = () => {
  const [email, setEmail] = useState(() => (localStorage.checkbox ? localStorage.email : ""));
  const [password, setPassword] = useState(() => (localStorage.checkbox ? localStorage.password : ""));
  const [isChecked, setIsChecked] = useState(() => !!localStorage.checkbox);
  const [showPassword, setShowPassword] = useState(false);

  const navigate = useNavigate();

  const handlePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (isChecked) {
      localStorage.email = email;
      localStorage.password = password;
      localStorage.checkbox = isChecked ? "1" : "";
    } else {
      localStorage.removeItem('email');
      localStorage.removeItem('password');
      localStorage.removeItem('checkbox');
    }

    try {
      console.log('Submitting:', { email, password });
      const response = await axios.post('https://localhost:7249/account/login', {
        email,
        password
      }, {
        headers: {
          'Content-Type': 'application/json'
        }
      });
      console.log('Response:', response.data);
      navigate("/");
      alert('Login successful!');
    } catch (error) {
      console.error(error);
      alert('Login failed. Please try again.');
    }
  };

  return (
    <div className="container">
      <div className="header">
        <div className="text">Welcome Back</div>
        <div className="underline"></div>
      </div>
      <form onSubmit={handleSubmit}>
        <div className="inputs">
          <div className="input">
            <i>
              <FontAwesomeIcon icon={faEnvelope} />
            </i>
            <input 
              type="email" 
              placeholder="Email" 
              value={email}
              onChange={(e) => setEmail(e.target.value)} 
            />
          </div>
        </div>
        <div className="inputs">
          <div className="input">
            <i>
              <FontAwesomeIcon icon={faLock} />
            </i>
            <input 
              type={showPassword ? "text" : "password"} 
              placeholder="Password" 
              value={password}
              onChange={(e) => setPassword(e.target.value)} 
            />
            <i onClick={handlePasswordVisibility} className='toggle-password'>
              <FontAwesomeIcon icon={showPassword ? faEyeSlash : faEye}/>
            </i>
          </div>
        </div>
        <div className="remember-me">
          <input
            type="checkbox"
            checked={isChecked}
            name="lsRememberMe"
            onChange={(e) => setIsChecked(e.target.checked)}
          />
          <label htmlFor="lsRememberMe"> Remember me</label>
        </div>
        <div className="forgot-password">Forgot Password? <Link to={'/Forgotpassword'}>Click Here!</Link></div>
        <div className="submit-container">
          <button type="submit" className="submit">Login</button>
        </div>
      </form>
    </div>
  );
};

export default Login;

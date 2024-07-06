import React, { useState } from 'react';
import axios from 'axios';
import {  faEnvelope } from '@fortawesome/free-regular-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser, faLock , faEye, faEyeSlash} from '@fortawesome/free-solid-svg-icons';
import { useNavigate } from "react-router-dom";
import './ResetPassword.css';

const ForgotPassword = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');
    const [showPassword, setShowPassword] = useState(false);

    const navigate = useNavigate();

    const handlePasswordVisibility = () => {
      setShowPassword(!showPassword);
    };

    const handleSubmit = async (e) =>{
        e.preventDefault();
        setMessage('');
        setError('');

        try {
            console.log('Submiting:', {email});
            const response = await axios.post('https://localhost:7249/Account/forgotpassword',{
                email                
              }, {
                headers: {
                  'Content-Type': 'application/json'
                }
              });
              console.log('Response:', response.data);
              navigate("/");              
            alert('If the email is registered in our system you will receive reser passowrd link on this email!')
        } catch (error) {
            console.log(error.message);
            alert('An error occurred while requesting the password reset.');
        }
    }
    
    return (
      <div className="container">
        <div className="header">
          <div className="text">
            You can easily change your password from this page!
          </div>
        </div>

        <div>
          <p>Please enter your email address and type your new passsword and then click Submit!</p>
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
                  required
                />
              </div>
              <div className="input">
            <i>
              <FontAwesomeIcon icon={faLock} />
            </i>
            <input 
              type={showPassword ? "text" : "password"} 
              placeholder="New Password" 
              value={password}
              onChange={(e) => setPassword(e.target.value)} 
            />
            <i onClick={handlePasswordVisibility} className='toggle-password'>
              <FontAwesomeIcon icon={showPassword ? faEyeSlash : faEye}/>
            </i>
          </div>
          <div className="input">
            <i>
              <FontAwesomeIcon icon={faLock} />
            </i>
            <input 
              type={showPassword ? "text" : "password"} 
              placeholder="Confirm Password" 
              value={password}
              onChange={(e) => setPassword(e.target.value)} 
            />
            <i onClick={handlePasswordVisibility} className='toggle-password'>
              <FontAwesomeIcon icon={showPassword ? faEyeSlash : faEye}/>
            </i>
          </div>
            </div>
            <div className="submit-container">
              <button type="submit" className="submit">
                Reset Password
              </button>
            </div>
          </form>
          {message && <p>{message}</p>}
          {error && <p style={{ color: "red" }}>{error}</p>}
        </div>
      </div>
    );  

};
export default ForgotPassword;
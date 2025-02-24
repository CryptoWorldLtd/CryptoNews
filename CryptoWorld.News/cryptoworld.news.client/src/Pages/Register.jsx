import React, { useState } from 'react';
import { useNavigate } from "react-router-dom";
import './Register.module.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser, faEnvelope } from '@fortawesome/free-regular-svg-icons';
import { faLock } from '@fortawesome/free-solid-svg-icons';
import axios from 'axios';
import { toast } from 'react-hot-toast';

const Register = () => {
  const [action] = useState("Register");
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState(null);
    
  const navigate = useNavigate();

  const handleUsernameChange = (event) => {
    setUsername(event.target.value);
  };

  const handleEmailChange = (event) => {
    setEmail(event.target.value);
  };

  const handlePasswordChange = (event) => {
    setPassword(event.target.value);
  };

  const handleConfirmPasswordChange = (event) => {
    setConfirmPassword(event.target.value);
  };

  const handleSubmit = async () => {
    setError(null); 
    try {
      console.log('Submitting:', { username, email, password, confirmPassword });
      const response = await axios.post('https://localhost:7249/account/register', {
        username,
        email,
        password,
        confirmPassword
      }, {
        headers: {
          'Content-Type': 'application/json'
        }
      });
        console.log('Response:', response.data);
        navigate("/Login");
        toast.success('Successful registration!');
    } catch (error) {
      console.error('Error:', error.response ? error.response.data : error.message);
      const errorMessage = error.response?.data?.detail || 'Registration failed. Please try again.';
      toast.error(errorMessage);
    }
  };

  return (
    <div className='container'>
      <div className="header">
        <div className='text'>{action}</div>
        <div className='underline'></div>
      </div>
      <div className='inputs'>
        <div className='input-group'>
          <div className='input'>
            <i>
              <FontAwesomeIcon icon={faUser} />
            </i>
            <input 
              type='text' 
              placeholder='Name' 
              value={username} 
              onChange={handleUsernameChange} 
            />
          </div>
        </div>
        
        <div className='input-group'>
          <div className='input'>
            <i>
              <FontAwesomeIcon icon={faEnvelope} />
            </i>
            <input 
              type='email' 
              placeholder='Email' 
              value={email} 
              onChange={handleEmailChange} 
            />
          </div>
        </div>
        <div className='input'>
          <i>
            <FontAwesomeIcon icon={faLock} />
          </i>
          <input 
            type='password' 
            placeholder='Password' 
            value={password}
            onChange={handlePasswordChange}
          />
        </div>
        <div className='input'>
          <i>
            <FontAwesomeIcon icon={faLock} />
          </i>
          <input 
            type='password' 
            placeholder='Confirm Password' 
            value={confirmPassword}
            onChange={handleConfirmPasswordChange}
          />
        </div>
      
        <div className='submit-container'>
          <div 
            className={action === "Login" ? "submit gray" : "submit"} 
            onClick={(handleSubmit)}
          >
            Register
          </div>
        </div>
        {error && <div className="error">{error}</div>}
      </div>
    </div>
  );
}

export default Register;

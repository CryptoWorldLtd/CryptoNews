import React, { useState } from 'react';
import { useNavigate } from "react-router-dom";
import './Register.module.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faLock } from '@fortawesome/free-solid-svg-icons';
import axios from 'axios';

const ChangePassword = () => {
  const [action] = useState("Change Password");
  const [password] = useState("");
  const [newPassword] = useState("");
  const [confirmNewPassword] = useState("");
  const [error, setError] = useState(null);
    
  const navigate = useNavigate();

  const handleConfirmPasswordChange = (event) => {
    setConfirmNewPassword(event.target.value);
  };

  const handleSubmit = async () => {
    setError(null); 
    try {
      const response = await axios.post('https://localhost:7249/account/edit', {
        password,
        newPassword,
        confirmNewPassword
      }, {
        headers: {
          'Content-Type': 'application/json'
        }
      });
        navigate("/Login");
        
    } catch (error) {
      console.error('Error:', error.response ? error.response.data : error.message);
      setError(error.response?.data?.message || "Password change failed");
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
              <FontAwesomeIcon icon={faLock} />
            </i>
            <input 
              type='password' 
              placeholder='Current Password' 
              value={password}  
            />
          </div>
        </div>
        <div className='input'>
          <i>
            <FontAwesomeIcon icon={faLock} />
          </i>
          <input 
            type='password' 
            placeholder='New Password' 
            value={newPassword}
          />
        </div>
        <div className='input'>
          <i>
            <FontAwesomeIcon icon={faLock} />
          </i>
          <input 
            type='password' 
            placeholder='Confirm new Password' 
            value={confirmNewPassword}
            onChange={handleConfirmPasswordChange}
          />
        </div>
      
        <div className='submit-container'>
          <div 
            className={action === "Login" ? "submit gray" : "submit"} 
            onClick={(handleSubmit)}
          >
            Save
          </div>
        </div>
        {error && <div className="error">{error}</div>}
      </div>
    </div>
  );
}

export default ChangePassword;
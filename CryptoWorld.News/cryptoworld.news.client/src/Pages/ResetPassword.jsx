import React, { useState } from "react";
import axios from "axios";
import { faEnvelope } from "@fortawesome/free-regular-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {faLock,faEye,faEyeSlash,} from "@fortawesome/free-solid-svg-icons";
import { useParams , useNavigate } from "react-router-dom";
import "./ResetPassword.css";

const ForgotPassword = () => {  
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");
  const [showPassword, setShowPassword] = useState(false);

  const {id} = useParams();
  const {email} = useParams();

  const navigate = useNavigate();
  
  const handlePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage("");
    setError("");

    const data = {
        email: email, 
        token: decodeURIComponent(id),       
        newPassword: password,
        confirmPassword: confirmPassword
    };

    try {
      console.log("Submiting:", data);
      const response = await axios.post(
        "https://localhost:7249/Account/resetpassword", 
        data);
      console.log("Response:", response.data);
      navigate("/login");
      alert(
        "Your password is successfully changed!"
      );
    } catch (error) {
      console.log(error.message);
      alert("An error occurred while requesting the password change!");
    }
  };

  return (
    <div className="container">
      <div className="header">
        <div className="text">
          You can easily change your password from this page!
        </div>
      </div>

      <div>
        <p>
          Please enter your email address and type your new passsword and then
          click Submit!
        </p>
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
                readOnly
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
              <i onClick={handlePasswordVisibility} className="toggle-password">
                <FontAwesomeIcon icon={showPassword ? faEyeSlash : faEye} />
              </i>
            </div>
            <div className="input">
              <i>
                <FontAwesomeIcon icon={faLock} />
              </i>
              <input
                type={showPassword ? "text" : "password"}
                placeholder="Confirm Password"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
              />
              <i onClick={handlePasswordVisibility} className="toggle-password">
                <FontAwesomeIcon icon={showPassword ? faEyeSlash : faEye} />
              </i>
            </div>
          </div>
          <div className="submit-container">
            <button type="submit" className="submit">
              Change Password
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

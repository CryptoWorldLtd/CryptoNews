import React from 'react';
import { useLocation } from 'react-router-dom';
import ChangeEmail from './ChangeEmail';
import ChangePassword from './ChangePassword';

const EditProfile = () => {
  const location = useLocation();
  const formType = location.pathname.includes('ChangeEmail') ? 'email' : 'password';

  return (
    <div>
      {formType === 'email' ? <ChangeEmail /> : <ChangePassword />}
    </div>
  );
};

export default EditProfile;
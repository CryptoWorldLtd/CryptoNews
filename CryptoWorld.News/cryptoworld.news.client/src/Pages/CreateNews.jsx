import React, { useState } from 'react';
import styles from './CreateNews.module.css';
import axios from 'axios';
import { toast } from 'react-hot-toast';

const CreateNews = () => {
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");
  const [imageUrl, setImageUrl] = useState("");
  const [datePublished, setDatePublished] = useState("");
  const [error, setError] = useState(null);

  const handleTitleChange = (event) => {
    setTitle(event.target.value);
  };

  const handleContentChange = (event) => {
    setContent(event.target.value);
  };

  const handleImageUrlChange = (event) => {
    setImageUrl(event.target.value);
  };

  const handleDatePublishedChange = (event) => {
    setDatePublished(event.target.value);
  };

  const handleSubmit = async () => {
    setError(null);
    try {
      console.log('Submitting:', { title, content, imageUrl, datePublished });
      const response = await axios.post('https://localhost:7249/createNews', {
        title,
        content,
        imageUrl,
        datePublished
      }, {
        headers: {
          'Content-Type': 'application/json'
        }
      });
      console.log('Response:', response.data);
    } catch (error) {
      console.error('Error:', error.response ? error.response.data : error.message);
      const errorMessage = error.response?.data?.detail || 'The news is not submitted.';
      toast.error(errorMessage);
    }
  };

  return (
    <div className={styles.wrapper}>
      <div className={styles.headerMain}>
        <div className={styles.textMain}>Create News</div>
        <div className={styles.underlineMain}></div>
      </div>
      <div className={styles.inputFields}>
        <div className={styles.inputGroup}>
          <div className={styles.inputField}>
            <input 
              type='text' 
              placeholder='Title' 
              value={title} 
              onChange={handleTitleChange} 
            />
          </div>
        </div>
        
        <div className={styles.inputGroup}>
          <div className={styles.inputField}>
            <textarea
              placeholder='Content' 
              value={content} 
              onChange={handleContentChange} 
            />
          </div>
        </div>

        <div className={styles.inputGroup}>
          <div className={styles.inputField}>
            <input 
              type='text' 
              placeholder='ImageUrl' 
              value={imageUrl}
              onChange={handleImageUrlChange}
            />
          </div>
        </div>

        <div className={styles.inputGroup}>
          <div className={styles.inputField}>
            <input 
              type='text' 
              placeholder='Date format: "dd.MM.yyyy HH:mm:ss"' 
              value={datePublished}
              onChange={handleDatePublishedChange}
            />
          </div>
        </div>

        <div className={styles.submitContainer}>
          <button className={styles.submit} onClick={handleSubmit}>
            Submit News
          </button>
        </div>
        {error && <div className={styles.error}>{error}</div>}
      </div>
    </div>
  );
}

export default CreateNews;

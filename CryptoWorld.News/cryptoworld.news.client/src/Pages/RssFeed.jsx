

import React, { useState } from 'react';
import axios from 'axios';

const RssFeed = () => {
    const [feedItems, setFeedItems] = useState([]);
    const [url, setUrl] = useState('');
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);

    const fetchFeed = async () => {
        console.log("Fetch feed called");
        if (!url) {
            setError("URL is required");
            return;
        }

        setLoading(true);
        setError(null);
        try {
            const response = await axios.get('https://localhost:7249/RssFeed/rssFeed', {
                params: { url }
            });
            console.log("Response data:", response.data);
            setFeedItems(response.data);
        } catch (error) {
            if (error.response) {
                console.error('Response error', error.response.data);
                setError(`Error fetching the RSS feed: ${error.response.data}`);
            } else if (error.request) {
                console.error('Request error', error.request);
                setError('Error fetching the RSS feed: No response from server');
            } else {
                console.error('General error', error.message);
                setError(`Error fetching the RSS feed: ${error.message}`);
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <input
                type="text"
                value={url}
                onChange={(e) => setUrl(e.target.value)}
                placeholder="Enter RSS feed URL"
            />
            <button onClick={fetchFeed}>Fetch Feed</button>
            {loading && <p>Loading...</p>}
            {error && <p>{error}</p>}
            <ul>
                {feedItems.map((item, index) => (
                    <li key={index}>
                        <h3><a href={item.link} target="_blank" rel="noopener noreferrer">{item.title}</a></h3>
                        <p><strong>Published on: </strong>{new Date(item.publishDate).toLocaleString()}</p>
                        <p>{item.description}</p>
                        {item.content && <div dangerouslySetInnerHTML={{ __html: item.content }} />}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default RssFeed;
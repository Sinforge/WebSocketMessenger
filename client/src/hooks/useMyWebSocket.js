import { useEffect } from 'react';
import { w3cwebsocket } from 'websocket';

const useWebSocket = (url, token, options) => {
  useEffect(() => {
    const socket = new w3cwebsocket(
      url + "?token=" + token,
      null,
      null,
      {
        'authorization' : 'Bearer ' + token,
        'accept-encoding' : 'gzip',
        'sec-websocket-version': 14
      },
      null,
      null,
  );

    socket.onopen = () => {
      if (options.onOpen) {
        options.onOpen();
      }
    };

    return () => {
      socket.close();
    };
  }, [url, token, options]);
};

export default useWebSocket;
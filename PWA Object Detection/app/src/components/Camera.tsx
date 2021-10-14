import React from "react";
import Webcam from "react-webcam";

export const Camera = ({ imageId }: { imageId: string }) => {
  const webcamRef = React.useRef(null);
  const videoConstraints = {
    height: 720,
    width: 1280,
    facingMode: "environment",
  };

  return (
    <div style={{ position: "relative" }}>
      <Webcam
        audio={false}
        id={imageId}
        ref={webcamRef}
        width={"100%"}
        //screenshotQuality={1}
        screenshotFormat="image/jpeg"
        videoConstraints={videoConstraints}
      />
      <div
        id="highlighter"
        style={{
          position: "absolute",
          left: 0,
          top: 0,
        }}
      ></div>
    </div>
  );
};

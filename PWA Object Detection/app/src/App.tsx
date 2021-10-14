import "./App.css";
import { Button, Flex, Segment } from "@fluentui/react-northstar";
import { Camera } from "./components";
import { useModel } from "./hooks/useModel";
import { useEffect, useState } from "react";

const IMAGE_ID = "webcamImageId";

enum AppState {
  Welcome,
  Running,
}

function App() {
  const [appState, setAppState] = useState(AppState.Welcome);

  const { loading, startDetection } = useModel(IMAGE_ID);

  useEffect(() => {
    if (!loading && appState === AppState.Running) {
      startDetection();
    }
  }, [loading, startDetection, appState]);

  if (loading) {
    return <h1>loading....</h1>;
  }

  return (
    <Flex column>
      {appState === AppState.Welcome ? (
        <>
          <Segment color="brand" inverted>
            Proud PH - Waste Detector
          </Segment>
          <Segment>
            <Button
              content="Start"
              onClick={() => setAppState(AppState.Running)}
            />
          </Segment>
        </>
      ) : (
        <Flex.Item>
          <Segment style={{ padding: "0", border: 0 }}>
            <Camera imageId={IMAGE_ID} />
          </Segment>
        </Flex.Item>
      )}
    </Flex>
  );
}

export default App;

import { useState, type FunctionComponent } from "react";
import "./BaseMarkerComponent.css";
import { AdvancedMarker } from "@vis.gl/react-google-maps";
import classNames from "classnames";
import type { BaseModel } from "../../Services/BaseService";

interface Props {
  base: BaseModel;
  activeMarkerId: string | null;
  setMarkerBaseId: (id: string | null) => void;
}

export const BaseMarkerComponent: FunctionComponent<Props> = ({
  base,
  activeMarkerId: activeMarkerId,
  setMarkerBaseId: setActiveMarkerId,
}) => {
  const [hovered, setHovered] = useState(false);

  const position = {
    lat: base.data.address.y,
    lng: base.data.address.x,
  };

  const isClicked = activeMarkerId === base.data.baseId;

  const handleClick = () => {
    if (isClicked) {
      setActiveMarkerId(null);
    } else {
      setActiveMarkerId(base.data.baseId);
    }
  };
  

  const renderCustomPin = () => {
    return (
      <>
        <div className="custom-pin">
          <button className="close-button">
            <span className="material-symbols-outlined"> close </span>
          </button>

          <div className="image-container" onClick={handleClick}>
            {isClicked ? (
              <span className="icon">
                <i className="bi bi-x-lg"></i>
              </span>
            ) : (
              <span className="icon">
                <i className="bi bi-house-fill"></i>
              </span>
            )}
          </div>

          <div className="details-container">
            <div className="marker-content">
              <h2>{base.data.name}</h2>
            </div>

            <div className="tab-bar">
              <button className="tab-button" style={{borderRadius: "0 6px 0 0"}}>
                <i className="bi bi-truck"></i>
                <br></br>
                <small className="tab-button-info fst-italic">
                  {base.data.storage.vehicles.length +
                    base.data.storage.defaultVehicles.length}
                </small>
              </button>
              <hr />
              <button className="tab-button">
                <i className="bi bi-person"></i>
                <br></br>
                <small className="tab-button-info fst-italic">
                  {base.data.storage.employees.length +
                    base.data.storage.defaultEmployees.length}
                </small>
              </button>
              <hr />
              <button className="tab-button">
                <i className="bi bi-wrench"></i>
                <br></br>
                <small className="tab-button-info fst-italic">
                  {base.data.storage.tools.length +
                    base.data.storage.defaultTools.length}
                </small>
              </button>
              <hr />
              <button className="tab-button" style={{borderRadius: "0 0 6px 0"}}>
                <i className="bi bi-truck-front"></i>
                <br></br>
                <small className="tab-button-info fst-italic">
                  {base.data.storage.machines.length +
                    base.data.storage.defaultMachines.length}
                </small>
              </button>
            </div>
          </div>
        </div>

        <div className="tip" />
      </>
    );
  };

  return (
    <>
      <AdvancedMarker
        position={position}
        onMouseEnter={() => setHovered(true)}
        onMouseLeave={() => setHovered(false)}
        className={classNames("custom-base-marker", { clicked: isClicked, hovered })}
        zIndex={isClicked ? 1000 : hovered ? 500 : 1}
      >
        {renderCustomPin()}
      </AdvancedMarker>
    </>
  );
};

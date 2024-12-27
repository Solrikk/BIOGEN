# DestructibleObject

A Unity script that allows GameObjects, such as cubes, to be destructible. When the object takes damage and its health drops below specified thresholds, it breaks into smaller fragments, creating realistic destruction effects with physics interactions.

## Table of Contents

- [Description](#description)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
  - [Setting Up the Destructible Object](#setting-up-the-destructible-object)
  - [Creating Fragment Prefabs](#creating-fragment-prefabs)
  - [Configuring Fragment Groups](#configuring-fragment-groups)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)

## Description

The `DestructibleObject` script enables Unity GameObjects to exhibit dynamic destruction behavior. When the object sustains damage and its health falls below defined thresholds, it fragments into smaller pieces. Each fragment interacts with the physics engine, allowing for realistic scattering and collision effects within the environment.

## Features

- **Health Management:** Define and track the maximum and current health of objects.
- **Fragment Groups:** Organize fragments into groups that activate at specific health thresholds.
- **Physics-Based Destruction:** Fragments are instantiated with Rigidbody components and respond to physics forces for realistic movement.
- **Customizable:** Easily configure fragment prefabs, health thresholds, and the number of fragments per group.
- **Debugging Support:** Logs destruction events and warnings for missing components to aid in troubleshooting.

## Installation

1. **Import into Unity:**
   - Open your Unity project.
   - Navigate to `Assets > Import Package > Custom Package` and select the downloaded package, or manually copy the `DestructibleObject` script and related assets into your project's `Assets` folder.

## Usage

### Setting Up the Destructible Object

1. **Create or Select a GameObject:**
   - Choose the GameObject you want to make destructible, such as a Cube.

2. **Add the `DestructibleObject` Script:**
   - In the Inspector panel, click on `Add Component`.
   - Search for `DestructibleObject` and add it to the GameObject.

3. **Configure Health:**
   - Set the `Max Health` value to define the object's maximum health.

### Creating Fragment Prefabs

1. **Create a Fragment:**
   - Duplicate your original GameObject and rename it to something like `CubeFragment`.

2. **Remove the `DestructibleObject` Script:**
   - Select `CubeFragment` in the Hierarchy.
   - In the Inspector, locate the `DestructibleObject` component and remove it to prevent recursive destruction.

3. **Add Necessary Components:**
   - **Collider:** Ensure the fragment has a Collider component (e.g., `BoxCollider`) for physical interactions.
   - **Rigidbody:** Add a `Rigidbody` component to enable physics behavior. Ensure `Use Gravity` is enabled and `Is Kinematic` is disabled.
   - **MeshCollider (Optional):** If using a `MeshCollider`, ensure the `Convex` option is enabled for compatibility with dynamic Rigidbody.

4. **Scale the Fragment:**
   - Adjust the `Scale` of the fragment to be smaller than the original object for a realistic effect.

5. **Create a Prefab:**
   - Drag the configured `CubeFragment` from the Hierarchy into your Project window to create a prefab.
   - Remove `CubeFragment` from the Hierarchy to keep the scene organized.

### Configuring Fragment Groups

1. **Select the Destructible Object:**
   - In the Hierarchy, select your original GameObject with the `DestructibleObject` script attached.

2. **Configure Fragment Groups:**
   - In the Inspector, locate the `Fragment Groups` array.
   - For each group:
     - **Health Threshold:** Set the health value at which this group of fragments will activate (e.g., 75, 50, 25). Ensure thresholds are in descending order.
     - **Fragment Prefab:** Drag and drop your `CubeFragment` prefab into this field.
     - **Fragment Count:** Specify how many fragments to instantiate when the threshold is reached (e.g., 4).

   ![Fragment Group Configuration](https://example.com/fragment-group-config.png)

## Troubleshooting

### Fragments Not Visible

- **Renderer Enabled:** Ensure that the fragment prefabs have active `Renderer` components.
- **Layer Visibility:** Check that fragments are on layers visible to the camera.
- **Scale Issues:** Verify that fragment prefabs are appropriately scaled and not too small or too large.
- **Positioning:** Ensure fragments are instantiated above the floor and not inside it.

### Fragments Falling Through the Floor

- **Colliders:** Ensure both the floor and fragments have properly configured colliders.
- **MeshCollider Settings:** Use primitive colliders like `BoxCollider` instead of `MeshCollider` when possible.
- **Collision Detection Mode:** Set Rigidbody’s `Collision Detection` to `Continuous` to prevent tunneling.
- **Physics Settings:** Check Unity’s physics settings to ensure appropriate collision layers.

### Recursion Issues

- **Remove `DestructibleObject` from Fragments:** Ensure that fragment prefabs do not have the `DestructibleObject` script attached, preventing recursive destruction.

### Debugging Steps

1. **Check Console Logs:** Ensure that log messages indicate fragment instantiation.
2. **Use Scene View:** During play mode, observe the Scene view to see if fragments are being created.
3. **Verify Prefab Configuration:** Ensure fragment prefabs have necessary components (`Collider`, `Rigidbody`, `Renderer`).
4. **Check Script Assignment:** Confirm that the correct script version is attached to the destructible object.

## Contributing

Contributions are welcome! Please follow these steps:

1. **Fork the Repository**
2. **Create a Feature Branch**
3. **Commit Your Changes**
4. **Push to the Branch**
5. **Open a Pull Request**

## License

This project is licensed under the [MIT License](LICENSE).

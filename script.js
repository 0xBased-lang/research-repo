/**
 * THOMA GmbH - Interactive Website Script
 * Modern, performant, and accessible JavaScript
 */

(function() {
    'use strict';

    // ============================================
    // DOM ELEMENTS
    // ============================================

    const navbar = document.getElementById('navbar');
    const mobileMenuToggle = document.querySelector('.mobile-menu-toggle');
    const navMenu = document.querySelector('.nav-menu');
    const navLinks = document.querySelectorAll('.nav-link');
    const scrollToTopBtn = document.getElementById('scrollToTop');
    const contactForm = document.getElementById('contactForm');

    // ============================================
    // MOBILE MENU TOGGLE
    // ============================================

    if (mobileMenuToggle) {
        mobileMenuToggle.addEventListener('click', () => {
            mobileMenuToggle.classList.toggle('active');
            navMenu.classList.toggle('active');

            // Update aria-label for accessibility
            const isExpanded = navMenu.classList.contains('active');
            mobileMenuToggle.setAttribute('aria-label', isExpanded ? 'Menu schließen' : 'Menu öffnen');
        });
    }

    // Close mobile menu when clicking on a nav link
    navLinks.forEach(link => {
        link.addEventListener('click', () => {
            if (window.innerWidth <= 768) {
                mobileMenuToggle.classList.remove('active');
                navMenu.classList.remove('active');
            }
        });
    });

    // Close mobile menu when clicking outside
    document.addEventListener('click', (e) => {
        if (window.innerWidth <= 768) {
            if (!e.target.closest('.nav-wrapper')) {
                mobileMenuToggle.classList.remove('active');
                navMenu.classList.remove('active');
            }
        }
    });

    // ============================================
    // SMOOTH SCROLL FOR ANCHOR LINKS
    // ============================================

    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function(e) {
            const href = this.getAttribute('href');

            // Don't prevent default for empty hash or just '#'
            if (href === '#' || href === '') return;

            const targetElement = document.querySelector(href);
            if (targetElement) {
                e.preventDefault();
                const navbarHeight = navbar ? navbar.offsetHeight : 80;
                const targetPosition = targetElement.offsetTop - navbarHeight;

                window.scrollTo({
                    top: targetPosition,
                    behavior: 'smooth'
                });
            }
        });
    });

    // ============================================
    // NAVBAR SCROLL EFFECT
    // ============================================

    let lastScroll = 0;
    const scrollThreshold = 100;

    function handleNavbarScroll() {
        const currentScroll = window.pageYOffset;

        if (currentScroll > scrollThreshold) {
            navbar.classList.add('scrolled');
        } else {
            navbar.classList.remove('scrolled');
        }

        lastScroll = currentScroll;
    }

    // Throttle scroll events for performance
    let ticking = false;
    window.addEventListener('scroll', () => {
        if (!ticking) {
            window.requestAnimationFrame(() => {
                handleNavbarScroll();
                handleScrollToTopButton();
                ticking = false;
            });
            ticking = true;
        }
    });

    // ============================================
    // ACTIVE NAVIGATION LINK ON SCROLL
    // ============================================

    function updateActiveNavLink() {
        const sections = document.querySelectorAll('section[id]');
        const navbarHeight = navbar ? navbar.offsetHeight : 80;
        const scrollPos = window.pageYOffset + navbarHeight + 100;

        sections.forEach(section => {
            const sectionTop = section.offsetTop;
            const sectionHeight = section.offsetHeight;
            const sectionId = section.getAttribute('id');

            if (scrollPos >= sectionTop && scrollPos < sectionTop + sectionHeight) {
                navLinks.forEach(link => {
                    link.classList.remove('active');
                    if (link.getAttribute('href') === `#${sectionId}`) {
                        link.classList.add('active');
                    }
                });
            }
        });
    }

    window.addEventListener('scroll', () => {
        if (!ticking) {
            window.requestAnimationFrame(() => {
                updateActiveNavLink();
            });
        }
    });

    // ============================================
    // SCROLL TO TOP BUTTON
    // ============================================

    function handleScrollToTopButton() {
        if (window.pageYOffset > 300) {
            scrollToTopBtn.classList.add('visible');
        } else {
            scrollToTopBtn.classList.remove('visible');
        }
    }

    if (scrollToTopBtn) {
        scrollToTopBtn.addEventListener('click', () => {
            window.scrollTo({
                top: 0,
                behavior: 'smooth'
            });
        });
    }

    // ============================================
    // DARK MODE TOGGLE
    // ============================================

    function initDarkMode() {
        // Check for saved theme preference or default to light mode
        const currentTheme = localStorage.getItem('theme') || 'light';
        document.documentElement.setAttribute('data-theme', currentTheme);

        // Create dark mode toggle button (can be added to nav if desired)
        const darkModeToggle = createDarkModeToggle();

        // Optional: Add to navbar
        // navbar.querySelector('.nav-wrapper').appendChild(darkModeToggle);
    }

    function createDarkModeToggle() {
        const button = document.createElement('button');
        button.className = 'dark-mode-toggle';
        button.setAttribute('aria-label', 'Toggle dark mode');
        button.innerHTML = `
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <circle cx="12" cy="12" r="5"/>
                <path d="M12 1v2M12 21v2M4.22 4.22l1.42 1.42M18.36 18.36l1.42 1.42M1 12h2M21 12h2M4.22 19.78l1.42-1.42M18.36 5.64l1.42-1.42"/>
            </svg>
        `;

        button.addEventListener('click', () => {
            const currentTheme = document.documentElement.getAttribute('data-theme');
            const newTheme = currentTheme === 'light' ? 'dark' : 'light';

            document.documentElement.setAttribute('data-theme', newTheme);
            localStorage.setItem('theme', newTheme);
        });

        return button;
    }

    // Initialize dark mode on page load
    initDarkMode();

    // ============================================
    // INTERSECTION OBSERVER FOR SCROLL ANIMATIONS
    // ============================================

    function initScrollAnimations() {
        const observerOptions = {
            threshold: 0.1,
            rootMargin: '0px 0px -100px 0px'
        };

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('visible');
                    // Optionally unobserve after animation
                    // observer.unobserve(entry.target);
                }
            });
        }, observerOptions);

        // Observe all elements with animation classes
        const animatedElements = document.querySelectorAll(
            '.fade-in, .scale-in, .slide-in-left, .slide-in-right, .service-card, .reference-card, .expertise-item'
        );

        animatedElements.forEach(el => {
            observer.observe(el);
        });
    }

    // Initialize scroll animations
    if ('IntersectionObserver' in window) {
        initScrollAnimations();
    }

    // ============================================
    // ANIMATED STATISTICS COUNTER
    // ============================================

    function animateCounter(element, target, duration = 2000) {
        const start = 0;
        const increment = target / (duration / 16); // 60fps
        let current = start;

        const timer = setInterval(() => {
            current += increment;
            if (current >= target) {
                current = target;
                clearInterval(timer);
            }

            // Format number with + suffix if original had it
            const hasPlus = element.textContent.includes('+');
            const hasPercent = element.textContent.includes('%');

            let displayValue = Math.floor(current);
            if (hasPlus) displayValue += '+';
            if (hasPercent) displayValue += '%';

            element.textContent = displayValue;
        }, 16);
    }

    function initCounterAnimations() {
        const counters = document.querySelectorAll('.stat-number');

        const counterObserver = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting && !entry.target.dataset.animated) {
                    const text = entry.target.textContent;
                    const number = parseInt(text.replace(/\D/g, ''));

                    entry.target.dataset.animated = 'true';
                    animateCounter(entry.target, number);
                }
            });
        }, { threshold: 0.5 });

        counters.forEach(counter => {
            counterObserver.observe(counter);
        });
    }

    // Initialize counter animations
    if ('IntersectionObserver' in window) {
        initCounterAnimations();
    }

    // ============================================
    // FORM VALIDATION & SUBMISSION
    // ============================================

    if (contactForm) {
        // Real-time validation
        const formInputs = contactForm.querySelectorAll('input, textarea, select');

        formInputs.forEach(input => {
            input.addEventListener('blur', () => {
                validateField(input);
            });

            input.addEventListener('input', () => {
                if (input.classList.contains('error')) {
                    validateField(input);
                }
            });
        });

        // Form submission
        contactForm.addEventListener('submit', async (e) => {
            e.preventDefault();

            // Validate all fields
            let isValid = true;
            formInputs.forEach(input => {
                if (!validateField(input)) {
                    isValid = false;
                }
            });

            if (!isValid) {
                showNotification('Bitte füllen Sie alle Pflichtfelder korrekt aus.', 'error');
                return;
            }

            // Get form data
            const formData = new FormData(contactForm);
            const data = Object.fromEntries(formData);

            // Show loading state
            const submitButton = contactForm.querySelector('button[type="submit"]');
            const originalText = submitButton.innerHTML;
            submitButton.disabled = true;
            submitButton.innerHTML = '<span>Wird gesendet...</span>';

            try {
                // Simulate API call (replace with your actual endpoint)
                await simulateFormSubmission(data);

                // Success
                showNotification('Vielen Dank! Ihre Nachricht wurde erfolgreich gesendet.', 'success');
                contactForm.reset();

            } catch (error) {
                // Error
                showNotification('Es gab ein Problem beim Senden Ihrer Nachricht. Bitte versuchen Sie es später erneut.', 'error');
                console.error('Form submission error:', error);

            } finally {
                // Reset button
                submitButton.disabled = false;
                submitButton.innerHTML = originalText;
            }
        });
    }

    function validateField(field) {
        const value = field.value.trim();
        const type = field.type;
        const required = field.hasAttribute('required');

        // Remove previous error
        removeFieldError(field);

        // Check if required field is empty
        if (required && !value) {
            addFieldError(field, 'Dieses Feld ist erforderlich.');
            return false;
        }

        // Email validation
        if (type === 'email' && value) {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(value)) {
                addFieldError(field, 'Bitte geben Sie eine gültige E-Mail-Adresse ein.');
                return false;
            }
        }

        // Phone validation (optional but if provided should be valid)
        if (type === 'tel' && value) {
            const phoneRegex = /^[\d\s\+\-\(\)]+$/;
            if (!phoneRegex.test(value)) {
                addFieldError(field, 'Bitte geben Sie eine gültige Telefonnummer ein.');
                return false;
            }
        }

        // Checkbox validation
        if (type === 'checkbox' && required && !field.checked) {
            addFieldError(field, 'Bitte akzeptieren Sie die Datenschutzerklärung.');
            return false;
        }

        return true;
    }

    function addFieldError(field, message) {
        field.classList.add('error');

        // Create error message if it doesn't exist
        let errorMsg = field.parentElement.querySelector('.error-message');
        if (!errorMsg) {
            errorMsg = document.createElement('span');
            errorMsg.className = 'error-message';
            errorMsg.style.cssText = 'color: var(--error); font-size: 0.875rem; margin-top: 0.25rem; display: block;';
            field.parentElement.appendChild(errorMsg);
        }
        errorMsg.textContent = message;

        // Add visual error indicator
        field.style.borderColor = 'var(--error)';
    }

    function removeFieldError(field) {
        field.classList.remove('error');
        field.style.borderColor = '';

        const errorMsg = field.parentElement.querySelector('.error-message');
        if (errorMsg) {
            errorMsg.remove();
        }
    }

    function simulateFormSubmission(data) {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                console.log('Form data:', data);
                // Simulating successful submission
                resolve({ success: true });

                // To simulate error, use:
                // reject(new Error('Submission failed'));
            }, 1500);
        });
    }

    // ============================================
    // NOTIFICATION SYSTEM
    // ============================================

    function showNotification(message, type = 'info') {
        // Create notification element
        const notification = document.createElement('div');
        notification.className = `notification notification-${type}`;
        notification.style.cssText = `
            position: fixed;
            top: 100px;
            right: 20px;
            max-width: 400px;
            padding: 1rem 1.5rem;
            background: var(--bg-primary);
            border-left: 4px solid ${type === 'success' ? 'var(--success)' : 'var(--error)'};
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-xl);
            z-index: var(--z-modal);
            animation: slideInRight 0.3s ease-out;
            font-size: 0.9375rem;
            color: var(--text-primary);
        `;

        notification.innerHTML = `
            <div style="display: flex; align-items: center; gap: 0.75rem;">
                <span style="font-size: 1.5rem;">${type === 'success' ? '✓' : '⚠'}</span>
                <p style="margin: 0; flex: 1;">${message}</p>
                <button onclick="this.parentElement.parentElement.remove()" style="
                    background: none;
                    border: none;
                    font-size: 1.5rem;
                    cursor: pointer;
                    color: var(--text-tertiary);
                    padding: 0;
                    line-height: 1;
                ">×</button>
            </div>
        `;

        document.body.appendChild(notification);

        // Auto-remove after 5 seconds
        setTimeout(() => {
            notification.style.animation = 'slideOutRight 0.3s ease-out';
            setTimeout(() => notification.remove(), 300);
        }, 5000);
    }

    // Add notification animations
    const style = document.createElement('style');
    style.textContent = `
        @keyframes slideInRight {
            from {
                opacity: 0;
                transform: translateX(100px);
            }
            to {
                opacity: 1;
                transform: translateX(0);
            }
        }

        @keyframes slideOutRight {
            from {
                opacity: 1;
                transform: translateX(0);
            }
            to {
                opacity: 0;
                transform: translateX(100px);
            }
        }
    `;
    document.head.appendChild(style);

    // ============================================
    // 3D CARD TILT EFFECT (Optional Enhancement)
    // ============================================

    function init3DCardEffect() {
        const cards = document.querySelectorAll('.service-card, .visual-card');

        cards.forEach(card => {
            card.addEventListener('mousemove', handleCardMouseMove);
            card.addEventListener('mouseleave', handleCardMouseLeave);
        });
    }

    function handleCardMouseMove(e) {
        const card = e.currentTarget;
        const rect = card.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const y = e.clientY - rect.top;

        const centerX = rect.width / 2;
        const centerY = rect.height / 2;

        const rotateX = (y - centerY) / 10;
        const rotateY = (centerX - x) / 10;

        card.style.transform = `perspective(1000px) rotateX(${rotateX}deg) rotateY(${rotateY}deg) translateZ(10px)`;
    }

    function handleCardMouseLeave(e) {
        const card = e.currentTarget;
        card.style.transform = 'perspective(1000px) rotateX(0) rotateY(0) translateZ(0)';
    }

    // Initialize 3D card effect (optional - can be enabled/disabled)
    // init3DCardEffect();

    // ============================================
    // LAZY LOADING IMAGES
    // ============================================

    function initLazyLoading() {
        const images = document.querySelectorAll('img[data-src]');

        const imageObserver = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const img = entry.target;
                    img.src = img.dataset.src;
                    img.removeAttribute('data-src');
                    imageObserver.unobserve(img);
                }
            });
        });

        images.forEach(img => imageObserver.observe(img));
    }

    // Initialize lazy loading
    if ('IntersectionObserver' in window) {
        initLazyLoading();
    }

    // ============================================
    // PERFORMANCE MONITORING (DEV MODE)
    // ============================================

    function logPerformanceMetrics() {
        if (window.performance && console.table) {
            const perfData = window.performance.timing;
            const pageLoadTime = perfData.loadEventEnd - perfData.navigationStart;
            const connectTime = perfData.responseEnd - perfData.requestStart;
            const renderTime = perfData.domComplete - perfData.domLoading;

            console.log('🚀 Performance Metrics:');
            console.table({
                'Page Load Time': `${pageLoadTime}ms`,
                'Connect Time': `${connectTime}ms`,
                'Render Time': `${renderTime}ms`,
            });
        }
    }

    // Log performance on page load (dev mode only)
    if (window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1') {
        window.addEventListener('load', () => {
            setTimeout(logPerformanceMetrics, 0);
        });
    }

    // ============================================
    // ACCESSIBILITY ENHANCEMENTS
    // ============================================

    function initAccessibility() {
        // Add skip to main content link
        const skipLink = document.createElement('a');
        skipLink.href = '#home';
        skipLink.className = 'skip-link';
        skipLink.textContent = 'Zum Hauptinhalt springen';
        skipLink.style.cssText = `
            position: absolute;
            top: -40px;
            left: 0;
            background: var(--primary-500);
            color: white;
            padding: 8px;
            text-decoration: none;
            z-index: 100;
        `;
        skipLink.addEventListener('focus', () => {
            skipLink.style.top = '0';
        });
        skipLink.addEventListener('blur', () => {
            skipLink.style.top = '-40px';
        });
        document.body.insertBefore(skipLink, document.body.firstChild);

        // Keyboard navigation for cards
        const interactiveCards = document.querySelectorAll('.service-card, .reference-card');
        interactiveCards.forEach(card => {
            card.setAttribute('tabindex', '0');
            card.addEventListener('keypress', (e) => {
                if (e.key === 'Enter') {
                    const link = card.querySelector('a');
                    if (link) link.click();
                }
            });
        });
    }

    initAccessibility();

    // ============================================
    // INITIALIZATION COMPLETE
    // ============================================

    console.log('✅ THOMA GmbH website initialized successfully');

    // Expose useful functions for debugging (dev mode only)
    if (window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1') {
        window.ThomaDebug = {
            showNotification,
            animateCounter,
            logPerformanceMetrics
        };
    }

})();
